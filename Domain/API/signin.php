<?php 

    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $publicKey = $_POST['publicKey'];
    
    if($publicKey==null)
    {
        die();
    }
    
    require_once "getHash.php";
    $login = GetHash($login);
    $password = GetHash($password);

    require_once "connect.php";
    
    function insert($user = [])
    {
        $array_level['id'] = $user[0];
        $array_level['public_name'] = $user[1];
        $array_level['public_key'] = $user[2];
        $array_level['key_number'] = $user[3];
        $array_level['changes_index'] = $user[4];
        return $array_level;
    }
    
    
    
    
    
    //----------------------------------------------------------------------------------------------
    $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $resultCount = $stmt->fetch();
    
    

    $keyNumb = $resultCount["key_number"];
    
    settype($keyNumb, 'integer');
    
    $keyNumb++;
    
    try {
        $stmt = $pdo->query("UPDATE `Users` SET `public_key` = '".$publicKey."', `key_number` = '".$keyNumb."' WHERE `Users`.`id` =".$resultCount["id"]);
    } catch (Exception $e) {
        echo "error";
    }
    
    
    
    
    
    $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $result = $stmt->fetch();

    $array_level = insert([$result["id"],$result["public_name"],$result["public_key"],$result["key_number"],$result["changes_index"]]);
    $json = json_encode($array_level);
    
    
    //$stmt = $pdo->query("DELETE FROM Messages WHERE `from_whom` = ".$result["id"]." OR `for_whom` = ".$result["id"]);
    
    echo $json;
    
    //-----------------------------------------------------------------------------------------------

    
?>
