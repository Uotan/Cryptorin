<?php
    $string = $_GET['hash'];
    require_once "getHash.php";
    if($string!=null)
    {
        $date = GetHash("$string");
        echo $date;
    }
    
?>