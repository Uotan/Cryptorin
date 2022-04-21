<?php 
    $login = addslashes($_POST['login']);
    
    if($login!=null){
        require_once "connect.php";
    
    $stmt = $pdo->query("SELECT COUNT(*) FROM `Users` WHERE `login` = '".$login."'");
    
    $resultCount = $stmt->fetch();
    $count = $resultCount["COUNT(*)"];
    
    
    settype($count, 'integer');
    
    if($count > 0){
        echo "exists";
    }
    else{
        echo "ok";
    }
    }
    
    
?>




