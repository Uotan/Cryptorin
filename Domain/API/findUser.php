<?php 

    $id = addslashes($_POST['id']);
    
    if($id!=null)
    {
        settype($id, 'integer');
    
        require_once "connect.php";
    
        function insert($user = [])
        {
            $array_level['id'] = $user[0];
            $array_level['public_name'] = $user[1];
            $array_level['public_key'] = $user[2];
            $array_level['key_number'] = $user[3];
            return $array_level;
        }
    
        $stmt = $pdo->query("SELECT * FROM `Users` WHERE id = ".$id);
        $resultCount = $stmt->fetch();

        $array_level = insert([$resultCount["id"],$resultCount["public_name"],$resultCount["public_key"],$resultCount["key_number"]]);
        $json = json_encode($array_level);
        echo $json;
    }
    
    
    
    
?>
