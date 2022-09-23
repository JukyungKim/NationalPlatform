
var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();

function login()
{
    pw = document.getElementById("pw").value;
    connection.invoke("LoginResult", pw).catch(function (err){
        return console.error(err.toString());
    });
}

history.pushState(null, null, location.href); 
window.onpopstate = function(event) { 
	history.go(1); 
};

setInterval(() => {
    logout();
}, 1000 * 60 * 10);

function logout()
{
    window.location.href = "home/login";
}

connection.start().then(function(){

}).catch(function(err){
    return console.error(err.toString());
})

connection.on("LoginError", function(result){
    // pw = document.getElementById("pw1").value;

    if(result === 0){
        alert("계정 정보가 정확하지 않습니다.");   
    }
});
