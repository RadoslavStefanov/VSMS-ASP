window.onload = (event) => applyFilter();

let tableRows = document.querySelector(".table").querySelectorAll("tr");
let dateInput = document.getElementById("start");

dateInput.addEventListener("change", applyFilter);

function showAll()
{
    for (let i = 0; i < tableRows.length; i++)
    {tableRows[i].style.display = "table-row";}
}


function applyFilter()
{
    showAll();
    let split = dateInput.value.split("-");
    let year = split[0];
    let month = split[1];
    let day = split[2];

    let argument = day + "." + month + "." + year;

    for (let i = 1; i < tableRows.length; i++)
    {
        let date = tableRows[i].querySelector(".date").innerText.split(" ")[0];
        if (date != argument)
        {tableRows[i].style.display = "none";}
    }
}