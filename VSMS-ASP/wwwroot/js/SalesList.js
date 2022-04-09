window.onload = (event) => applyFilter();

let tableRows = document.querySelector(".table").querySelectorAll("tr");
let dateInput = document.getElementById("start");
let TotalDisplay = document.getElementById("TotalDisplay");
let tempTotal = 0;

dateInput.addEventListener("change", applyFilter);

function showAll()
{
    tempTotal = 0;
    for (let i = 1; i < tableRows.length; i++)
    {
        tableRows[i].style.display = "table-row";
        console.log(tableRows[i]);
        tempTotal = parseFloat(tempTotal) + parseFloat(tableRows[i].querySelector(".total").innerText);
    }
    TotalDisplay.innerText = "Оборот: " + tempTotal + " лв.";
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
        {
            tableRows[i].style.display = "none";
            tempTotal = parseFloat(tempTotal) - parseFloat(tableRows[i].querySelector(".total").innerText);
        }
    }
    TotalDisplay.innerText = "Оборот: " + tempTotal + " лв.";
}