let delIcons = document.getElementsByClassName("delete-btn");
for (let i = 0; i < delIcons.length; i++) { delIcons[i].addEventListener('click', addHref) }

let addNewBTN = document.getElementById("add-category-btn");
let addNewCatName = document.getElementById("add-category-field");

addNewBTN.addEventListener('click',addNewCategory)

function addHref(e)
{
	let modalDelBTN = document.getElementById("delete");
	let catgName = e.target.parentNode.parentNode.parentNode.querySelector("td:nth-of-type(2)").innerText;
	modalDelBTN.setAttribute("href", "/Categories/Delete/?arg=" + catgName);
}

function addNewCategory()
{
	if (addNewCatName.value.length > 4 && addNewCatName.value.length <= 25 )
	{ window.location.replace("/Categories/Create?arg=" + addNewCatName.value); }
	else
	{ alert("Името на новата категория трябва да бъде между 5 и 25 символа!"); }
}
