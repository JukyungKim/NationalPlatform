var connection = new signalR.HubConnectionBuilder().withUrl("/accountHub").build();
var selectedImagefile = false;

const uploadButton = documnet.getElementById('upload_button');



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

connection.on("PlanId", function(result){
    // pw = document.getElementById("pw1").value;
    if(result === true){
        alert("도면 이름이 중복됩니다.");
    }
});

function checkPlanId()
{    
    console.log("Plan id 추가 " + document.getElementById("plan_id").value);

    planId = document.getElementById("plan_id").value;

    if(selectedImagefile === false || planId === ""){
        alert("도면 이미지 파일 선택, 도면 이름 입력은 필수입니다.")
        return;
    }

    connection.invoke("CheckPlanId", planId).catch(function (err){
        return console.error(err.toString());
    });
}

function checkFileUpload(file){
    console.log(file);
    var fileLength = file.length;
    var fileDot = file.lastIndexOf(".");
    var fileType = file.substring(fileDot+1, fileLength).toLowerCase();
    

    if(fileType != "jpg" && fileType != "png"){
        alert("jpg, png 파일을 선택하세요.")
    }
    else{
        selectedImagefile = true;
    }
}

