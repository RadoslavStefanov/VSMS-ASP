let passwordControl = document.getElementById("passwordControl");
passwordControl.addEventListener('click', showHidePassword)

let passwordField = document.getElementById("password");

function showHidePassword() {
    if (passwordField.type == "password") { passwordField.type = "text"; passwordControl.innerText = "Скрии паролата" }
    else { passwordField.type = "password"; passwordControl.innerText = "Покажи паролата" }
}