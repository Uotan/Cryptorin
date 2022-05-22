<?php 

    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $fromID = addslashes($_POST['fromID']);
    $toID = addslashes($_POST['toID']);
    
    
    settype($fromID, 'integer');
    settype($toID, 'integer');
    
    require_once "getHash.php";
    $login = GetHash($login);
    $password = GetHash($password);
    
    require_once "connect.php";
    

    $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $resultCount = $stmt->fetch();
    
    if($resultCount!=null){
        $sqlCountQuery = "SELECT COUNT(*) FROM Messages WHERE from_whom = '".$fromID."' and for_whom = '".$toID."'";
        $STH_count  = $pdo->query($sqlCountQuery);
    $resultCount = $STH_count->fetch();
    $countOfMessagesOnDB = $resultCount["COUNT(*)"];
    
    settype($countOfMessagesOnDB, 'integer');
        echo $countOfMessagesOnDB;
    }
    else{
        echo "error";
    }

    
    
    
?>
