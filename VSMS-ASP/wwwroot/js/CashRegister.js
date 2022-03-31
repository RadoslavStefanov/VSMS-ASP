let uploadButtons = document.getElementsByClassName("saleBtn");
let minusButtons = document.getElementsByClassName("taker");
let plusButtons = document.getElementsByClassName("adder");
let saleModal = document.getElementById("saleModal");
let amountInput = document.getElementById("amountInput");
let sellButton = document.getElementById("sell");
let receiptTable = document.getElementById("table");

let inputProduct =
{
    imageUrl: null,
    Name: null,
    Price: null
}

amountInput.addEventListener('input', preventChar)
sell.addEventListener('click', sendToReceipt)
for (let i = 0; i < uploadButtons.length; i++)
{ uploadButtons[i].addEventListener('click', loadProduct2Modal) }

for (let i = 0; i < minusButtons.length; i++)
{ minusButtons[i].addEventListener('click', removeAmount) }

for (let i = 0; i < plusButtons.length; i++)
{ plusButtons[i].addEventListener('click', addAmount) }


function preventChar()
{
    if (isNaN(amountInput.value))
    {
        alert("В това поле могат да се записват само числа!");
        amountImput.value = null;
    }
    else if (amountInput.value < 0)
    {
        alert("Броя не може да бъде по-малък от 0");
        amountImput.value = null;
    }
}

function loadProduct2Modal(e)
{
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

function addAmount(e)
{
    let curNumber = amountInput.value
    if (isNaN(curNumber) || curNumber==null || curNumber.length==0)
    {
        curNumber = 0;
        curNumber =  parseInt(e.target.value)
        amountInput.value = curNumber;
    }
    else
    {
        curNumber = parseInt(amountInput.value)
        curNumber = parseInt(curNumber)+ parseInt(e.target.value)
        amountInput.value = curNumber;
    }
}

function removeAmount(e)
{
    let curNumber = amountInput.value - e.target.value
    if (curNumber < 0) {
        alert("Броя не може да бъде по-малък от 0");
    }
    else
    {
        amountInput.value = curNumber;
    }
    
}

function sendToReceipt(e)
{
    
    let outputProduct =
    {
        Name:saleModal.querySelector(".card-title").innerText = inputProduct.Name,
        Amout: amountInput.value,
        PricePP:saleModal.querySelector(".card-text").innerText = inputProduct.Price
    }

    if (outputProduct.Name == null
        || outputProduct.Amout == null
        || outputProduct.Amout <= 0
        || outputProduct.PricePP == null
        || outputProduct.PricePP <= 0) {
        alert("Входните данни не отговарят на изискванията");
    }
    else
    {
        let tr = document.createElement("tr");
        let td1 = document.createElement("td");
        let td2 = document.createElement("td");
        let td3 = document.createElement("td");
        let td4 = document.createElement("td");
            let a = document.createElement("a");
            let i = document.createElement("i");



        td1.textContent = outputProduct.Name;
        td2.textContent = outputProduct.Amout+"бр.";
        td3.textContent = (parseFloat(outputProduct.Amout) * parseFloat(outputProduct.PricePP)).toFixed(2)+"лв";

        i.classList.add("fa");
        i.classList.add("fa-trash");
        i.ariaHiddent = true;

        a.classList.add("delete-btn");
        a.appendChild(i);
        a.addEventListener('click',deleteTableItem)

        td4.appendChild(a);

        tr.appendChild(td1);
        tr.appendChild(td2);
        tr.appendChild(td3);
        tr.appendChild(td4);

        let table = document.getElementById("table");
        table.appendChild(tr);
    }

    function deleteTableItem(e)
    {

    }

    function updateTotal
    {

    }
}