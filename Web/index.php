<?php
if(isset($_GET["voice"])){
    file_put_contents("voice.txt",$_POST["voice"]);
    header("Location: ./");
}
?><!DOCTYPE html>
<html lang="ja">
<head> 
<meta name="viewport" content="user-scalable=no">
<meta charset="UTF-8">
<title>iTunes Music Station</title>
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>
<script src="./script.js"></script>
<script>
var nowplayer,music,album,artist,voice;
function online(){
$.getJSON("http://192.168.0.15/music/now.php",function(json){ 
if(json.music != music){
    music = json.music;
    document.getElementById("music").innerHTML=json.music;
    notify("[" + json.now + "]" + json.music + " (" + json.album + "/" + json.artist + ")");
}
if(json.album != album){
    album = json.album;
    document.getElementById("album").innerHTML=json.album;
}
if(json.artist != artist){
    artist = json.artist;
    document.getElementById("artist").innerHTML=json.artist;
}
if(json.now != nowplayer){
    nowplayer = json.now;
    document.getElementById("status").innerHTML=json.now;
    if(json.now == "start"){
        document.form.start.disabled = true;
        document.form.stop.disabled = false;
        document.getElementById("status").style.color = "blue";
    }else if(json.now == "stop"){
        document.form.start.disabled = false;
        document.form.stop.disabled = true;
        document.getElementById("status").style.color = "red";
    }else{
        document.getElementById("status").style.color = "yellow";
    }
}
if(json.voice != voice){
    document.voices.voice.value = json.voice;
    document.getElementById("voice").innerHTML = json.voice;
    voice = json.voice;
}
});
//画像のオブジェクトを作成し各イベントを定義する
	var img = new Image(0,0);
	    img.onerror = worksForErr;
	    img.onload  = worksForLoaded;

	    //画像を読み込む
	    img.src = "http://192.168.0.15/music/ts.png";
}
	//画像を読み込めたときの処理をここに書きます
	function worksForLoaded(e)
	{
          document.getElementById("statusimg").innerHTML="[ONLINE]";
          document.getElementById("statusimg").style.color = "blue";
	}
//画像を読み込めなかったときの処理をここに書きます
	function worksForErr(e)
	{
		  document.getElementById("statusimg").innerHTML="[OFFLINE]";
          document.getElementById("statusimg").style.color = "red";
	}
function nowload(now){
$.getJSON("http://192.168.0.15/music/reload.php?now=" + now,function(json){});
}
function musicload(){
voice = document.voices.voice.value;
$.getJSON("http://192.168.0.15/music/reload.php?voice=" + voice,function(json){});
}
setInterval("online()", 2000);
online();
</script>
<style>
body {
height: 1200px;
width: 900px;
background-color: #cccccc;
}
h2 {
position: relative;
top: 50px;
text-align:center;
font-size: 200%;
}
h3 {
position: relative;
top: 52px;
text-align:center;
font-size: 200%;
}
#status {
    color: yellow;
    font-weight: bold;
}
#statusimg,#small {
    font-size: 50%;
}
p.music {
position: relative;
top: 60px;
text-align:center;
font-size: 150%;
}
p.music:before {
content: "Song: ";
color:green;
}
p.album {
position: relative;
top: 62px;
text-align:center;
font-size: 150%;
}
p.album:before {
content: "Album: ";
color:green;
}
p.artist {
position: relative;
top: 64px;
text-align:center;
font-size: 150%;
}
p.artist:before {
content: "Artist: ";
color:green;
}
p.voice {
position: relative;
top: 68px;
text-align:center;
font-size: 150%;
}
p.voice:before {
content: "Voice: ";
color:green;
}
div.now {
position: relative;
top: 100px;
text-align:center;
}
div.skip {
position: relative;
top: 105px;
text-align:center;
}
div.voice {
position: relative;
top: 115px;
text-align:center;
}
button.submit {
width: 250px;
height: 230px;
font-size: 200%;
margin: 50px 90px 50px 90px;
}
</style>
</head>
<body>
<h2>iTunes Music Station</h2>
<h3>Status:<span id="status">Loading...</span><span id="small">/</span><span id="statusimg">Loading...</span></h3>
<p class="music" id="music">Loading...</p>
<p class="album" id="album">Loading...</p>
<p class="artist" id="artist">Loading...</p>
<p class="voice" id="voice">Loading...</p>
<form method="post" action="" name="form">
<div class="now">
<button type="button" onclick="nowload('start')" class="submit" id="start">Start</button>
<button type="button" onclick="nowload('stop')" class="submit" id="stop">Stop</button>
</div>
<div class="skip">
<button type="button" onclick="nowload('back')" class="submit" id="back">Back</button>
<button type="button" onclick="nowload('next')" class="submit" id="next">Next</button>
</div>
</form>
<form method="post" action="?voice" name="voices">
<div class="voice">
Voice:<input type="number" name="voice" min="0" max="100" id="voice">
<input type="submit" name="Voice Enter">
</div>
</form>
</body>
</html>
