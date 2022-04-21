<?php 

    $id = addslashes($_POST['id']);
    
    settype($fromID, 'integer');
    settype($toID, 'integer');
    
    require_once "connect.php";
    

    $stmt = $pdo->query("SELECT * FROM `Users` WHERE id = ".$id);
    $result = $stmt->fetch();

    $keyNumber = $result["key_number"];
    
    settype($keyNumber, 'integer');
    echo $keyNumber;


    
    
    
?>
