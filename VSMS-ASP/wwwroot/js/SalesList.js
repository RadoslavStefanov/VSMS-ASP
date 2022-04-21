window.onload = (event) => TurnLocalTimeZone();
window.onload = (event) => applyFilter();


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
        let input = (tableRows[i].querySelector(".total").innerText.split(" ")[0]).replace(',', '.')
        tempTotal = tempTotal + parseFloat(input);
    }
    TotalDisplay.innerText = "Оборот: " + tempTotal.toFixed(2) + " лв.";
}


function applyFilter() {
    showAll();
    let split = dateInput.value.split("-");
    let year = split[0];
    let month = split[1];
    let day = split[2];


    for (let i = 1; i < tableRows.length; i++) {
        let date = tableRows[i].querySelector(".utc-date").innerText;
        console.log(tableRows[i]);
        date = date.split(" ")[0];

        let separator = getSeparator(date);

        let dmonth = addLeadingZero(date.split(separator)[0]);
        let dday = addLeadingZero(date.split(separator)[1]);
        let dyear = date.split(separator)[2];

        console.log(date);
        if (dday != day || dmonth != month || dyear != year) {
            tableRows[i].style.display = "none";
            tempTotal = parseFloat(tempTotal) - parseFloat(tableRows[i].querySelector(".total").innerText);
        }
    }
    TotalDisplay.innerText = "Оборот: " + tempTotal.toFixed(2) + " лв.";
}

function addLeadingZero(input) {
    if (input.length < 2) { return ("0" + input) }
    return input;
}

function getSeparator(input)
{
    for (let i = 1; i < input.length; i++)
    {
        if (isNaN(input[i])) { return input[i];}
    }
}
