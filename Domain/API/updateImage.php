<?php
    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $imageBase64 = $_POST['image'];
    
    if($imageBase64==null)
    {
        die();
    }
    
    require_once "connect.php";
    
    
     $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $resultCount = $stmt->fetch();

    $imagePath = $resultCount["image_path"];
    if($imagePath!=null||$imagePath!="")
    {
        if(file_exists($imaePath))
        {
            unlink($imaePath);
        }
    }
    $img_file_path = "../images/".$resultCount["id"].".jpg";
    

    $bin = base64_decode($imageBase64);
    $im = imageCreateFromString($bin);
    if (!$im) {
        die('Base64 value is not a valid image');
    }
    
    $success = file_put_contents($img_file_path, $bin);
    exec('convert '.$img_file_path.' -resize 1024x1024 '.$img_file_path);

    $stmt = $pdo->query("UPDATE `Users` SET `image_path` = '".$img_file_path."' WHERE `Users`.`id` =".$resultCount["id"]);
    
    echo "Updated";
    //echo $imagePath;
    
    
?>