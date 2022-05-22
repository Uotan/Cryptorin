<?php
    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $newPublicName = addslashes($_POST['newPublicName']);
    
    if($login==null||$password==null||$newPublicName==null||strlen($newPublicName)>255)
    {
        die("Error");
    }
    
    require_once "getHash.php";
    $login = GetHash($login);
    $password = GetHash($password);
    
    require_once "connect.php";
    
    
    $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $result = $stmt->fetch();
    
    $changeIndex = $result["changes_index"];
    settype($changeIndex, 'integer');
    $changeIndex++;

    $userID = $result["id"];
    if($userID!=null||$userID!="")
    {
        $stmt = $pdo->query("UPDATE `Users` SET `public_name` = '".$newPublicName."' WHERE `Users`.`id` =".$userID);
        $stmt = $pdo->query("UPDATE `Users` SET `changes_index` = '".$changeIndex."' WHERE `Users`.`id` =".$userID);
    
    echo "Updated";
    }
    else
    {
        echo "Error";
    }

    

    
    
?>