import json
import pandas as pd
import os
import sys
from pypdf import PdfReader, PdfWriter
from pypdf.generic import NameObject, BooleanObject

import datetime
import math

with open("config.json", "r", encoding="utf-8") as f:
    cfg = json.load(f)

TEMPLATE_PATH = sys.argv[1]#cfg["template_path"]
EXCEL_PATH = sys.argv[2]#cfg["excel_path"]
EXCEL_HAS_HEADER = cfg.get("excel_has_header", False)
PDF_IDENTIFYING_FIELD = cfg["pdf_identifying_field"]

df = pd.read_excel(EXCEL_PATH, header=0 if EXCEL_HAS_HEADER else None)
def format_value(v):
    if isinstance(v, float) and math.isnan(v):
        return ""

    if isinstance(v, (pd.Timestamp, datetime.datetime, datetime.date)):
        return v.strftime("%d/%m/%Y")

    return str(v)

def build_fields_to_update(row):
    result = {}
    for pdf_field, rule in cfg["fields"].items():
        src = rule.get("source", "excel")

        if src == "excel":
            if "column_name" in rule:
                value = row[rule["column_name"]]
            else:                              
                value = row.iloc[rule["column_index"]]
        elif src == "config":
            value = rule.get("value", "")
        else:
            value = ""

        result[pdf_field] = format_value(value)
    return result


def fill_certificate(row, output_path):
    reader = PdfReader(TEMPLATE_PATH)
    writer = PdfWriter()
    writer.clone_reader_document_root(reader)

    fields_to_update = build_fields_to_update(row)

    for page in writer.pages:
        writer.update_page_form_field_values(page, fields_to_update)

    for page in writer.pages:
        if "/Annots" not in page:
            continue

        for annot_ref in page["/Annots"]:
            annot = annot_ref.get_object()
            if annot.get("/Subtype") != "/Widget":
                continue

            field_name = annot.get("/T")
            if not field_name:
                continue

            clean_name = str(field_name).strip("()")
            if clean_name in fields_to_update and "/AP" in annot:
                del annot["/AP"]

    acroform = writer._root_object["/AcroForm"]
    acroform.update({
        NameObject("/NeedAppearances"): BooleanObject(True)
    })

    with open(output_path, "wb") as f:
        writer.write(f)


for idx, row in df.iterrows():
    try:
        nome = build_fields_to_update(row)[PDF_IDENTIFYING_FIELD]
    except KeyError:
        nome = f"row_{idx+1}"

    safe_name = str(nome).replace(" ", "_")
    output_file =  os.path.join("output",f"cert_{idx+1}_{safe_name}.pdf")

    fill_certificate(row, output_file)
    print("Saved", output_file)
