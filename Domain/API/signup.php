<?php 
    $publicName = addslashes($_POST['publicName']);
    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $imageBase64 = $_POST['image'];


    $img_file_path = "";
    
    
    require_once "getHash.php";
    $login = GetHash($login);
    $password = GetHash($password);
    
    require_once "connect.php";
    
    
    if($login!=null&&$publicName!=null&&$password!=null){
    
    $stmt = $pdo->query("SELECT COUNT(*) FROM `Users` WHERE `login` = '".$login."'");
    
    $resultCount = $stmt->fetch();
    $count = $resultCount["COUNT(*)"];
    
    
    settype($count, 'integer');
    
    if($count == 0){
        
        
        
        
        $stmt1 = $pdo->query("INSERT INTO `Users` (`id`, `login`, `password`, `public_name`, `image_path`, `public_key`, `key_number`,`changes_index`) VALUES (NULL, '".$login."', '".$password."', '".$publicName."', NULL, NULL, '0', '0')");
        
        
        if($imageBase64!=null)
        {
            $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
            $resultId = $stmt->fetch();
            $img_file_path = "../images/".$resultId["id"].".jpg";
    

            $bin = base64_decode($imageBase64);
            $im = imageCreateFromString($bin);
            if (!$im) {
                die('Base64 value is not a valid image');
            }
    
            $success = file_put_contents($img_file_path, $bin);
            exec('convert '.$img_file_path.' -resize 1024x1024 '.$img_file_path);
        
        
            $stmt = $pdo->query("UPDATE `Users` SET `image_path` = '".$img_file_path."' WHERE `Users`.`id` =".$resultId["id"]);
        }
        
        
        echo "created";
    }
}