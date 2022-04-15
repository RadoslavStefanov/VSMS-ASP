let uploadButtons = document.getElementsByClassName("saleBtn");
let minusButtons = document.getElementsByClassName("taker");
let plusButtons = document.getElementsByClassName("adder");
let saleModal = document.getElementById("saleModal");
let amountInput = document.getElementById("amountInput");
let sellButton = document.getElementById("sell");
let receiptTable = document.getElementById("table");
let totalPriceField = document.getElementById("totalPrice");
let JSONoutput = document.getElementById("saleJSON");
let filterApplyButton = document.getElementById("filterApply");
let filterRemover = document.getElementById("filterRemover");
let body = document.querySelector("body");

let inputProduct =
{
    imageUrl: null,
    Name: null,
    Price: null
}

body.classList = "";
body.classList.add("sidebar-mini")
body.classList.add("layout-fixed")
body.classList.add("sidebar-closed")
body.classList.add("sidebar-collapse")


amountInput.addEventListener('input', preventChar)
sell.addEventListener('click', sendToReceipt)
filterRemover.addEventListener('click', removeFilter)

filterApplyButton.addEventListener('click', applyFilter)
for (let i = 0; i < uploadButtons.length; i++) { uploadButtons[i].addEventListener('click', loadProduct2Modal) }

for (let i = 0; i < minusButtons.length; i++) { minusButtons[i].addEventListener('click', removeAmount) }

for (let i = 0; i < plusButtons.length; i++) { plusButtons[i].addEventListener('click', addAmount) }


function preventChar() {
    if (isNaN(amountInput.value)) {
        alert("В това поле могат да се записват само числа!");
        amountImput.value = null;
    }
    else if (amountInput.value < 0) {
        alert("Броя не може да бъде по-малък от 0");
        amountImput.value = null;
    }
}

function loadProduct2Modal(e) {
    let card = e.target.parentNode.parentNode;

    inputProduct =
    {
        imageUrl: card.querySelector(".card-img-top").src,
        Name: card.querySelector(".card-title").innerText,
        Price: card.querySelector(".card-text").innerText
    }

    saleModal.querySelector(".card-img-top").src = inputProduct.imageUrl
    saleModal.querySelector(".card-title").innerText = inputProduct.Name
    saleModal.querySelector(".card-text").innerText = inputProduct.Price
    saleModal.querySelector(".secondName").innerText = inputProduct.Name
    amountInput.value = null;
}

function addAmount(e) {
    let curNumber = amountInput.value
    if (isNaN(curNumber) || curNumber == null || curNumber.length == 0) {
        curNumber = 0;
        curNumber = parseInt(e.target.value)
        amountInput.value = curNumber;
    }
    else {
        curNumber = parseInt(amountInput.value)
        curNumber = parseInt(curNumber) + parseInt(e.target.value)
        amountInput.value = curNumber;
    }
}

function removeAmount(e) {
    let curNumber = amountInput.value - e.target.value
    if (curNumber < 0) {
        alert("Броя не може да бъде по-малък от 0");
    }
    else {
        amountInput.value = curNumber;
    }

}

function deleteTableItem(e) {
    e.target.parentNode.parentNode.parentNode.remove();
    updateTotal();
}

function sendToReceipt(e) {
    let outputProduct =
    {
        Name: saleModal.querySelector(".card-title").innerText = inputProduct.Name,
        Amout: amountInput.value,
        PricePP: saleModal.querySelector(".card-text").innerText = inputProduct.Price
    }

    let table = document.getElementById("table");
    let curProducts = table.querySelectorAll("tr");
    let combine = false;

    for (let i = 0; i < curProducts.length; i++) {
        if (curProducts[i].querySelector(".Name").innerText == outputProduct.Name) {
            let value = curProducts[i].querySelector(".Amount").innerText.split('б')[0];
            let price = parseFloat(curProducts[i].querySelector(".Price").innerText.split('л')[0]);

            value = parseInt(parseInt(value) + parseInt(outputProduct.Amout));
            value = value + "бр."
            curProducts[i].querySelector(".Amount").innerText = value

            let curPricePP = parseFloat(outputProduct.PricePP.split("л")[0].replace(',', '.'))

            price = parseFloat(price) + parseFloat(parseInt(outputProduct.Amout) * curPricePP)
            curProducts[i].querySelector(".Price").innerText = price + "лв"
            combine = true;
        }
    }

    if (combine == false) {
        if (outputProduct.Name == null
            || outputProduct.Amout == null
            || outputProduct.Amout <= 0
            || outputProduct.PricePP == null
            || outputProduct.PricePP <= 0) {
            alert("Входните данни не отговарят на изискванията");
        }
        else {
            let tr = document.createElement("tr");
            let td1 = document.createElement("td");
            let td2 = document.createElement("td");
            let td3 = document.createElement("td");
            let td4 = document.createElement("td");
            let td5 = document.createElement("td");
            let a = document.createElement("a");
            let i = document.createElement("i");

            td5.style.display = "none";
            td5.textContent = outputProduct.PricePP;
            td5.classList.add("AtPrice");

            console.log(td5);

            td1.textContent = outputProduct.Name;
            td1.classList.add("Name");
            td2.textContent = outputProduct.Amout + "бр.";
            td2.classList.add("Amount");
            td3.textContent = (parseFloat((outputProduct.Amout).replace(',', '.')) * parseFloat((outputProduct.PricePP).replace(',', '.'))).toFixed(2) + "лв";
            td3.classList.add("Price");
            td3.classList.add("tdPrice");

            i.classList.add("fa");
            i.classList.add("fa-trash");
            i.ariaHiddent = true;

            a.classList.add("delete-btn");
            i.addEventListener('click', deleteTableItem)
            a.appendChild(i);


            td4.appendChild(a);

            tr.appendChild(td1);
            tr.appendChild(td2);
            tr.appendChild(td3);
            tr.appendChild(td4);
            tr.appendChild(td5);

            table.appendChild(tr);
        }
    }
    updateTotal();
}

function updateTotal() {
    let allPrices = document.getElementsByClassName("tdPrice");
    totalPriceField.value = 0.00;

    for (let i = 0; i < allPrices.length; i++) {
        let input = (allPrices[i].innerText.split('л')[0]);
        totalPriceField.value = (parseFloat((totalPriceField.value).replace(',', '.')) + parseFloat((input).replace(',', '.'))).toFixed(2);
    }

    updateJSON();
}

function updateJSON() {
    let array = [];
    let rows = receiptTable.querySelectorAll("tr")
    for (let i = 0; i < rows.length; i++) {
        let tempObj = new Object();
        tempObj.soldProductName = rows[i].querySelector(".Name").innerText;
        tempObj.soldProductAmout = parseInt(rows[i].querySelector(".Amount").innerText.split('б')[0]);
        tempObj.soldProductTotalPrice = parseFloat(((rows[i].querySelector(".Price").innerText.split('л')[0]).replace(',', '.')).replace(',', '.'));
        tempObj.AtPrice = parseFloat(((rows[i].querySelector(".AtPrice").innerText.split('л')[0]).replace(',', '.')).replace(',', '.'));
        array.push(tempObj);
    }
    JSONoutput.value = JSON.stringify(array);
}

function removeFilter() {
    let filerDiv = document.getElementById("filterDiv");
    filerDiv.style.display = "none";
    filerDiv.querySelector("p").innerText = "";
    let productsLister = document.getElementById("productsLister");
    let productsCards = productsLister.getElementsByClassName("card");
    showAllCards(productsLister, productsCards);
}

function showAllCards(productsLister, productsCards) {
    for (let i = 0; i < productsCards.length; i++) { productsCards[i].style.display = "inline-block"; }
}

function applyFilter() {
    let filterCatInput = document.getElementById("FilterCatSelector");
    let filterKiloInput = document.getElementById("FilterKiloSelector");
    let filerDiv = document.getElementById("filterDiv");
    let productsLister = document.getElementById("productsLister");
    let productsCards = productsLister.getElementsByClassName("card");

    if (filterCatInput.value == "null" && filterKiloInput.value == "null") { showAllCards(productsLister, productsCards); }

    if (filterCatInput.value != "null" && filterKiloInput.value != "null") {
        showAllCards(productsLister, productsCards);
        for (let i = 0; i < productsCards.length; i++) {
            if (productsCards[i].querySelector(".categoryName").innerText !== filterCatInput.value
                || productsCards[i].querySelector(".Kilograms").innerText !== filterKiloInput.value) { productsCards[i].style.display = "none"; }
        }
        filerDiv.style.display = "flow-root";
        filerDiv.querySelector("p").innerText = "Филтър: " + filterCatInput.value + " / " + filterKiloInput.value + "кг."
    }

    if (filterCatInput.value != "null" && filterKiloInput.value == "null") {
        showAllCards(productsLister, productsCards);
        for (let i = 0; i < productsCards.length; i++) {
            if (productsCards[i].querySelector(".categoryName").innerText !== filterCatInput.value) { productsCards[i].style.display = "none"; }
            filerDiv.style.display = "flow-root";
            filerDiv.querySelector("p").innerText = "Филтър: " + filterCatInput.value
        }
    }

    if (filterCatInput.value == "null" && filterKiloInput.value != "null") {
        showAllCards(productsLister, productsCards);
        for (let i = 0; i < productsCards.length; i++) {
            if (productsCards[i].querySelector(".Kilograms").innerText !== filterKiloInput.value) { productsCards[i].style.display = "none"; }
            filerDiv.style.display = "flow-root";
            filerDiv.querySelector("p").innerText = "Филтър: " + filterKiloInput.value + "кг."
        }
    }
}