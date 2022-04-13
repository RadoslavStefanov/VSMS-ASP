let imgUrlField = document.getElementById("imageUrlField");
let imgField = document.getElementById("avatar");
let priceField = document.getElementById("price");
let submitBTN = document.getElementById("submit");

submitBTN.disabled = "true"

imgUrlField.addEventListener('input', UpdateImage);
priceField.addEventListener('input', validatePrice);


function validatePrice() {
    if (priceField.value.length > 2) {
        var isInputCorrect = false;
        if (priceField.value !== null) {
            if (!isNaN(priceField.value.replace(',', '.'))) {
                isInputCorrect = true;
                submitBTN.disabled = null
            }
        }

        if (isInputCorrect == false) {
            submitBTN.disabled = "true";
            alert("Моля попълнете сума във формат '123.21'!");
            priceField.value = "";
        }
    }
}

function UpdateImage() {
    if (imgUrlField.value.length > 5) { imgField.src = imgUrlField.value; }
}