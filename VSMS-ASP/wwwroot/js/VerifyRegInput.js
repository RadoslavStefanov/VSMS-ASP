let pass = document.getElementById("Pass");
let confirmPass = document.getElementById("confirmPass");
let passAlert = document.getElementById("passAlert");

let btn = document.getElementById("submitBTN");
btn.disabled="true"
confirmPass.addEventListener('input',checkPasswords)

function checkPasswords()
{
    if (confirmPass.value.length>5)
    {
        if (confirmPass.value != pass.value) {
            passAlert.style.display = "block";
        }
        else
        {
            btn.disabled = null;
            passAlert.style.display = "none";
        }
    }
}