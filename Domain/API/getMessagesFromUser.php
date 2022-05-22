<?php 

    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $fromID = addslashes($_POST['fromID']);
    $toID = addslashes($_POST['toID']);
    $countNeed = addslashes($_POST['count']);
    
    settype($fromID, 'integer');
    settype($toID, 'integer');
    settype($countNeed, 'integer');
    
    require_once "getHash.php";
    $login = GetHash($login);
    $password = GetHash($password);
    
    require_once "connect.php";
    
    function insert($user = [])
    {
        $array_level['id'] = $user[0];
        $array_level['from_whom'] = $user[1];
        $array_level['for_whom'] = $user[2];
        $array_level['rsa_cipher'] = $user[3];
        $array_level['datetime'] = $user[4];
        return $array_level;
    }

    $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $resultCount = $stmt->fetch();
    
    if($resultCount!=null){
        
        $sqlCountQuery = "SELECT COUNT(*) FROM Messages WHERE from_whom = '".$fromID."' and for_whom = '".$toID."'";
        $STH_count  = $pdo->query($sqlCountQuery);
        $resultCount = $STH_count->fetch();
        $countOfMessagesOnDB = $resultCount["COUNT(*)"];
        
        settype($countOfMessagesOnDB, 'integer');
        $deltaMessages = $countOfMessagesOnDB - $countNeed;
    
        
        $sqlquery = "SELECT * FROM `Messages` WHERE from_whom = '".$fromID."' and for_whom = '".$toID."' LIMIT ".$deltaMessages.", ".$countNeed;
        $STH  = $pdo->query($sqlquery);
        $result = $STH->fetchAll();
        $array_level = array();
        foreach( $result as $row ) {
            $array_level[] = insert([$row["id"],$row["from_whom"],$row["for_whom"],$row["rsa_cipher"],$row["datetime"]]);
        }
        $json = json_encode($array_level);
        echo $json;
    }
    else{
        echo "error";
    }

    
    
    
?>
