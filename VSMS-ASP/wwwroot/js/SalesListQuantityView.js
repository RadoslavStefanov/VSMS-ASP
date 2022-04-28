function populateQuantities()
{
    let salesTableRows = document.querySelector("#salesTable").querySelectorAll("tr");
    let quantitiesTable = document.querySelector("#quantitiesTable");

    let dict = {};

    for (let i = 1; i < salesTableRows.length; i++)
    {
        quantitiesTable.querySelector("tbody").innerHTML = "";

        if (salesTableRows[i].style.display == "none") { continue }

        productName = salesTableRows[i].querySelector(".productName").innerText;
        if (dict[productName] == undefined) {
            dict[productName] =
            {
                "name": productName,
                "amountCount": parseInt(salesTableRows[i].querySelector(".quantity").innerText),
                "amountPP": parseInt(salesTableRows[i].querySelector(".kgPP").innerText),
                "totalPrice": parseFloat(salesTableRows[i].querySelector(".total").innerText),
                "totalKG": parseInt(salesTableRows[i].querySelector(".kgPP").innerText) * parseInt(salesTableRows[i].querySelector(".quantity").innerText),
            }
        }
        else
        {
            dict[productName].amountCount += parseInt(salesTableRows[i].querySelector(".quantity").innerText);
            dict[productName].totalPrice += parseFloat(salesTableRows[i].querySelector(".total").innerText);
            dict[productName].totalKG += parseInt(salesTableRows[i].querySelector(".kgPP").innerText) * parseInt(salesTableRows[i].querySelector(".quantity").innerText);
        }
    }

    let jsonArray = [];

    for (let i = 0; i < Object.entries(dict).length; i++)
    {
        let tr = document.createElement('tr');

        let dateTD = document.createElement('td');
        dateTD.innerText = document.getElementById("start").value;

        let pNameTD = document.createElement('td');
        pNameTD.innerText = Object.entries(dict)[i][0];

        let pAmountCountTD = document.createElement('td');
        pAmountCountTD.innerText = dict[Object.entries(dict)[i][0]].amountCount + " бр.";

        let amoutPpTD = document.createElement('td');
        amoutPpTD.innerText = dict[Object.entries(dict)[i][0]].amountPP + " кг.";

        let totalPriceTD = document.createElement('td');
        totalPriceTD.innerText = dict[Object.entries(dict)[i][0]].totalPrice.toFixed(2) + " лв.";

        let totalKgTD = document.createElement('td');
        totalKgTD.innerText = dict[Object.entries(dict)[i][0]].totalKG + " кг.";

        tr.appendChild(dateTD);
        tr.appendChild(pNameTD);
        tr.appendChild(pAmountCountTD);
        tr.appendChild(amoutPpTD);
        tr.appendChild(totalPriceTD);
        tr.appendChild(totalKgTD);

        quantitiesTable.querySelector("tbody").appendChild(tr);


        let tempObj = new Object();
        tempObj.productName = Object.entries(dict)[i][0];
        tempObj.soldAmount = dict[Object.entries(dict)[i][0]].totalKG + " кг.";
        tempObj.totalPrice = dict[Object.entries(dict)[i][0]].totalPrice.toFixed(2) + " лв.";
        jsonArray.push(tempObj);
    }
    var JSONresult = JSON.stringify(jsonArray);
    document.getElementById("quantityJSON").value = JSONresult + "*_*" + document.querySelector("#TotalDisplay").innerText;
}