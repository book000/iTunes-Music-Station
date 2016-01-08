<?php
if(isset($_GET["now"])){
   $now = $_GET["now"];
   file_put_contents("now.txt",$now);
}
