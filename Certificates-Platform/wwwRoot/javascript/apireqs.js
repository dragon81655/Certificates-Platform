
document.addEventListener("DOMContentLoaded", () => {
    let button = document.getElementById("uploadData");
    button.addEventListener("click", async (e) => {
        e.preventDefault();
        console.log("Works!");
        const fileInput = document.getElementById("pdf");
        const fileInput2 = document.getElementById("xlsx");
        if (!fileInput.files.length) return;
        if (!fileInput2.files.length) return;

        const formData = new FormData();
        formData.append("pdf", fileInput.files[0]);
        formData.append("exel", fileInput2.files[0]);

        const res = await fetch("https://localhost:7201/api/certs/upload", {
            method: "POST",
            body: formData
        });

        const data = await res.json();
        console.log(data);
        //document.getElementById("result").textContent = JSON.stringify(data, null, 2);
    });

    /*document.getElementById("uploadPDF").addEventListener("change", async () => {
        const filesAdded = document.getElementById("uploadedFiles");
        let t = "";
        const pdf = document.getElementById("pdf");
        for (let i = 0; i < pdf.files.length; i++) {
            t += `<li>${pdf.files[i].name} - ${Math.round(pdf.files[i].size)} B</li>`;
        };
        filesAdded.innerHTML = t;
    });*/

});