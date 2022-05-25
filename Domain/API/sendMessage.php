<?php 

    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $fromID = addslashes($_POST['fromID']);
    $toID = addslashes($_POST['toID']);
    $content = addslashes($_POST['content']);
    
    settype($fromID, 'integer');
    settype($toID, 'integer');
    
    
    require_once "getHash.php";
    require_once "getTime.php";
    $login = GetHash($login);
    $password = GetHash($password);
    
    require_once "connect.php";
    

    $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $resultCount = $stmt->fetch();
    
    $resultId = $resultCount["id"];
    
    if($resultCount!=null&&$resultId==$fromID){
        
        $date = GetTime();
        $stmt = $pdo->query("INSERT INTO `Messages` (`id`, `from_whom`, `for_whom`, `rsa_cipher`, `datetime`) VALUES (NULL, '".$fromID."', '".$toID."', '".$content."', '".$date."')");
        echo $date;
    }
    else{
        echo "error";
    }
?>