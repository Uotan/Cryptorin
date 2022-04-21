<?php 
    $id = addslashes($_POST['id']);

    require_once "connect.php";
    
    $stmt = $pdo->query("SELECT * FROM `Users` WHERE id = ".$id);
    $resultCount = $stmt->fetch();
    $img = file_get_contents($resultCount["image_path"]);
  
// Encode the image string data into base64
$data = base64_encode($img);
  
// Display the output
echo $data;
