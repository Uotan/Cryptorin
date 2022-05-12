<?php 

    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $publicKey = $_POST['publicKey'];
    
    if($publicKey==null)
    {
        die();
    }
    
    
    require_once "connect.php";

    
    
    
    
    
    //----------------------------------------------------------------------------------------------
    
    
    $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $resultCount = $stmt->fetch();
    
    if($resultCount==null)
    {
        echo "error";
        die();
    }

    $keyNumb = $resultCount["key_number"];
    
    
    $stmt = $pdo->query("DELETE FROM Messages WHERE `from_whom` = ".$resultCount["id"]." OR `for_whom` = ".$resultCount["id"]);
    
    settype($keyNumb, 'integer');
    
    $keyNumb++;
    
    try {
        $stmt = $pdo->query("UPDATE `Users` SET `public_key` = '".$publicKey."', `key_number` = '".$keyNumb."' WHERE `Users`.`id` =".$resultCount["id"]);
        echo $keyNumb;
    } 
    catch (Exception $e) 
    {
        echo "error";
    }
    //-----------------------------------------------------------------------------------------------

    
?>
