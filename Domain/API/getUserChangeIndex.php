<?php 

    $id = addslashes($_POST['id']);
    
    settype($id, 'integer');
    
    require_once "connect.php";
    

    $stmt = $pdo->query("SELECT `changes_index` FROM `Users` WHERE id = ".$id);
    $result = $stmt->fetch();

    $key = $result["changes_index"];
    
    echo $key;


    
    
    
?>
