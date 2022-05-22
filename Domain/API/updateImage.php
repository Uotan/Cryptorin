<?php

    //получаем методом POST данные для валидации и картинку в формате base64
    $login = addslashes($_POST['login']);
    $password = addslashes($_POST['password']);
    $imageBase64 = $_POST['image'];
    
    //просто проверяем, что картинка вообще отправлена
    if($imageBase64==null)
    {
        //если нет - прекратить работу скрипта
        die();
    }
    
    require_once "getHash.php";
    $login = GetHash($login);
    $password = GetHash($password);
    
    
    //подключаем скрипт подключения к базе
    //в нем будет переменная   $pdo
    require_once "connect.php";
    
    
    
    //получаем путь, где должна находиться картинка, из таблицы Users
    $stmt = $pdo->query("SELECT * FROM `Users` WHERE login = '".$login."' and password = '".$password."'");
    $resultCount = $stmt->fetch();
    
    
    $changeIndex = $resultCount["changes_index"];
    settype($changeIndex, 'integer');
    $changeIndex++;
    
    $imagePath = $resultCount["image_path"];
    
    //если ссылка на файл существует - удалим этот файл
    if($imagePath!=null||$imagePath!="")
    {
        if(file_exists($imaePath))
        {
            unlink($imaePath);
        }
    }
    
    //просто подготавливаем путь к файлу (string)
    $img_file_path = "../images/".$resultCount["id"].".jpg";
    
    //создаем массив байтов
    $bin = base64_decode($imageBase64);
    //пытаемся создать картинку из потока байтов, если не удается - прерываем выполнение скрипта
    $im = imageCreateFromString($bin);
    if (!$im) {
        die('Base64 value is not a valid image');
    }
    
    //помещает массив байтов в файл
    //Если filename не существует, файл создается. В противном случае существующий файл будет перезаписан
    $success = file_put_contents($img_file_path, $bin);
    
    //чтобы картинки разных размеров отображались нормально - принудительно увиличим по ширине и высоте
    exec('convert '.$img_file_path.' -resize 1024x1024 '.$img_file_path);


    //обновляем путь к файлу изображению, эта строчка по идее то и не нужна, но у меня предусмотренно, что пользователь
    //может не загружать картинку при регистрации
    //а значит поле `image_path` по умолчанию пустое
    $stmt = $pdo->query("UPDATE `Users` SET `image_path` = '".$img_file_path."' WHERE `Users`.`id` =".$resultCount["id"]);
    $stmt = $pdo->query("UPDATE `Users` SET `changes_index` = '".$changeIndex."' WHERE `Users`.`id` =".$resultCount["id"]);
    
    //возвращаю клиенту статус операции, там нужно будет сделать Trim(), удалив лишние пробелы
    //чтобы можно было сравнить строку так if(result=="Updated"){}
    echo "Updated";
    

    //-----------------------------------------------------------------------------------------------

?>