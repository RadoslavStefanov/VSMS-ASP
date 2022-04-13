window.onload = (event) => applyFilter();
window.onload = (event) => TurnLocalTimeZone();

let tableRows = document.querySelector(".table").querySelectorAll("tr");
let dateInput = document.getElementById("start");
let TotalDisplay = document.getElementById("TotalDisplay");
let tempTotal = 0;

dateInput.addEventListener("change", applyFilter);

function TurnLocalTimeZone() {
    for (let i = 1; i < tableRows.length; i++) {
        var date = tableRows[i].querySelector(".date");
        var utcValue = new Date(date.innerText);
        date.innerText = utcValue.toLocaleString();
    }
}


function showAll() {
    tempTotal = 0;
    for (let i = 1; i < tableRows.length; i++) {
        tableRows[i].style.display = "table-row";
        tempTotal = parseFloat(tempTotal) + parseFloat(tableRows[i].querySelector(".total").innerText);
    }
    TotalDisplay.innerText = "Оборот: " + tempTotal.toFixed(2) + " лв.";
}


function applyFilter() {
    showAll();
    let split = dateInput.value.split("-");
    let year = split[0];
    let month = split[1];
    let day = split[2];

    let argument = day + "." + month + "." + year;

    for (let i = 1; i < tableRows.length; i++) {
        let date = tableRows[i].querySelector(".date").innerText.split(" ")[0];
        if (date != argument) {
            tableRows[i].style.display = "none";
            tempTotal = parseFloat(tempTotal) - parseFloat(tableRows[i].querySelector(".total").innerText);
        }
    }
    TotalDisplay.innerText = "Оборот: " + tempTotal + " лв.";
}