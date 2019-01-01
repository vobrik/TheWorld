/* site.js */
var ele = document.getElementById("username");
ele.innerHTML = "Vla Guzman";

var main = document.getElementById("main");
main.onmouseenter = function () {
	main.style.backgroundColor = "#888";
}; 

main.onmouseleave = function () {
	main.style.backgroundColor = "";
};