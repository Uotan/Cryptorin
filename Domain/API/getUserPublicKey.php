<?php 

    $id = addslashes($_POST['id']);
    
    settype($id, 'integer');
    
    require_once "connect.php";
    

    $stmt = $pdo->query("SELECT `public_key` FROM `Users` WHERE id = ".$id);
    $result = $stmt->fetch();

    $key = $result["public_key"];
    
    echo $key;


    
    
    
?>
