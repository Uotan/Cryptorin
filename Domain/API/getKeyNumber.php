<?php 

    $id = addslashes($_POST['id']);
    
    settype($id, 'integer');
    
    require_once "connect.php";
    

    $stmt = $pdo->query("SELECT `key_number` FROM `Users` WHERE id = ".$id);
    $result = $stmt->fetch();

    $keyNumber = $result["key_number"];
    
    settype($keyNumber, 'integer');
    echo $keyNumber;


    
    
    
?>
