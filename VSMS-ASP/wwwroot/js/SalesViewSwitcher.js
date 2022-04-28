let QuantitySwitch = document.getElementById("quantitySwitch");
let SalesSwitch = document.getElementById("salesSwitch");


SalesSwitch.addEventListener('click', drawSales);
QuantitySwitch.addEventListener('click', drawQuantities);

function drawSales(e)
{
    if (!(e.target.classList.contains("active")))
    {
        document.getElementById("salesTable").style.display = "inline-table";
        document.getElementById("quantitiesTable").style.display = "none";
    }
}

function drawQuantities(e)
{
    if (!(e.target.classList.contains("active")))
    {
        document.getElementById("salesTable").style.display = "none";
        document.getElementById("quantitiesTable").style.display = "inline-table";
    }
}