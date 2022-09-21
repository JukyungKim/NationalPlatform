var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();

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

function checkPlanId()
{
    console.log("Plan id 추가 " + document.getElementById("plan_id").value);
    planId = document.getElementById("plan_id").value;
    connection.invoke("CheckPlanId", planId).catch(function (err){
        return console.error(err.toString());
    });
}

connection.on("PlanId", function(result){
    // pw = document.getElementById("pw1").value;
    if(result === true){
        alert("도면 이름이 중복됩니다.");
    }
});