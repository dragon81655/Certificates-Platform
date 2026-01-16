import os
import sys
import json
from pypdf import PdfReader

if getattr(sys, "frozen", False):
    BASE_DIR = os.path.dirname(sys.executable)
else:
    BASE_DIR = os.path.dirname(os.path.abspath(__file__))

def resolve_path(path):
    if os.path.isabs(path):
        return path
    return os.path.join(BASE_DIR, path)

def main():
    print("=== PDF FIELD DUMP ===\n")

    config_path = os.path.join(BASE_DIR, "config.json")
    if not os.path.isfile(config_path):
        print("ERROR: config.json not found in:", BASE_DIR)
        input("\nPress Enter to exit...")
        return

    with open(config_path, "r", encoding="utf-8") as f:
        cfg = json.load(f)

    template_path = resolve_path(cfg["template_path"])
    if not os.path.isfile(template_path):
        print("ERROR: template PDF not found at:", template_path)
        input("\nPress Enter to exit...")
        return

    reader = PdfReader(template_path)
    fields = reader.get_fields()

    if not fields:
        print("No form fields found in this PDF.")
        input("\nPress Enter to exit...")
        return

    print(f"Template: {template_path}\n")
    print("Fields and current values:")
    print("-" * 60)

    for name, field in fields.items():
        clean_name = str(name).strip("()")
        value = field.get("/V", "")
        print(f"Name : {clean_name}")
        print(f"Value: {value}")
        print("-" * 60)

    #input("\nPress Enter to exit...")

#if __name__ == "__main__":
main()
