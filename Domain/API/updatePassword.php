<?php
    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $newPassword = addslashes($_POST['newPassword']);
    
    if($login==null||$password==null||$newPassword==null)
    {
        die("Error");
    }
    
    require_once "connect.php";
    
    
    $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $result = $stmt->fetch();

    $userID = $result["id"];
    if($userID!=null||$userID!="")
    {
        $stmt = $pdo->query("UPDATE `Users` SET `password` = '".$newPassword."' WHERE `Users`.`id` =".$userID);
    
    echo "Updated";
    }
    else
    {
        echo "Error";
    }

    

    
    
?>