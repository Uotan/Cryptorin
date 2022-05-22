<?php
    function GetHash($secretString)
    {
        $options = ['salt' => '%Js#O~Bpkm@|HtHyfauYX%','cost' => 12];
        $hash = password_hash($secretString, PASSWORD_DEFAULT, $options);
        return $hash;
    }
    
?>