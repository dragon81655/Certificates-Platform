document.getElementById("uploadForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const fileInput = document.getElementById("pdf");
    if (!fileInput.files.length) return;

    const formData = new FormData();
    formData.append("file", fileInput.files[0]);

    const res = await fetch("https://localhost:7201/api/upload", {
        method: "POST",
        body: formData
    });

    const data = await res.json();
    console.log(data);
    //document.getElementById("result").textContent = JSON.stringify(data, null, 2);
});

document.getElementById("uploadForm").addEventListener("change", async () => {
    const filesAdded = document.getElementById("uploadedFiles");
    let t = "";
    const pdf = document.getElementById("pdf");
    for (let i = 0; i < pdf.files.length; i++) {
        t += `<li>${pdf.files[i].name} - ${Math.round(pdf.files[i].size)} B</li>`;
    };
    filesAdded.innerHTML = t;
});