
 window.onpopstate = function(event){
     if(event){
         console.log("뒤로가기");
         var link = "https://localhost:5001/home/main";
         location.href = link;
         location.replace(link);
         window.open(link);
     }
 }

 setInterval(() => {
    logout();
}, 1000 * 60 * 10);

function logout()
{
    window.location.href = "home/login";
}

function show() {
    document.querySelector(".background").className = "background btn_add_sensor";
}

function close() {
    document.querySelector(".background").className = "background";
}
function hello() {
    alert('Hello world3');
}

function open() {
    document.querySelector(".modal").classList.remove("hidden");
}

function close2(){
    document.querySelector(".modal").classList.add("hidden");
}

window.onload = function () {
    // alert('1');
    // document.getElementById('hw2').addEventListener('click', hello);

    document.querySelector(".btn_add_sensor").addEventListener('click', show);
    document.querySelector(".close").addEventListener('click', close);
    document.querySelector(".cancel").addEventListener('click', close);

    // document.querySelector(".openBtn").addEventListener("click", open);
    // document.querySelector(".closeBtn").addEventListener("click", close2);
    // document.querySelector(".bg").addEventListener("click", close2);
}













