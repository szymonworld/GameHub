<?php
 
require_once __DIR__ . '/db_connect.php';
 
$response = array();
$requestType = array("createAccount","createGame");
$selectedType = -1;
$db = null;
for ($i = 0; $i < sizeof($requestType); $i++) 
{

 if(isset($_POST['type']) == $requestType)
 {
	 $selectedType = $i; 
	 $db = new DB_CONNECT();
 }
 
}

switch ($selectedType) 
{
    case 0:
        echo "i equals 0";
        break;
    case 1:
        echo "i equals 1";
        break;
    case 2:
        echo "i equals 2";
        break;
	default:
		break;
	
}
 
 
function authenticate($login, $password)
{
	return password_verify($password, loadHashByLogin($login));
}
 
 
function loadHashByLogin($login)
{
	$query = $conn->prepare("SELECT Password FROM Accounts WHERE Login = :login");
	$query->bindValue(':login', $login);
	$query->execute();
	return $query->fetch();
}
 
function createGame()
{
	
	
}
 
 
 
 
 
 
 
 
 
// check for required fields
if (isset($_POST['name']) && isset($_POST['date']) && isset($_POST['description']) && isset($_POST['description'])) 
{
 
    $name = $_POST['name'];
    $price = $_POST['price'];
    $description = $_POST['description'];
 
    
 
    $db = new DB_CONNECT();
 
	$query = $conn->prepare("INSERT INTO tbl VALUES(:id, :name)");
	$query->bindValue(':id', $id);
	$query->bindValue(':name', $name);
	$query->execute();
	 
    // mysql inserting a new row
    $result = mysql_query("INSERT INTO products(name, price, description) VALUES('$name', '$price', '$description')");
 
    // check if row inserted or not
    if ($result) {
        // successfully inserted into database
        $response["success"] = 1;
        $response["message"] = "Product successfully created.";
 
        // echoing JSON response
        echo json_encode($response);
    } else {
        // failed to insert row
        $response["success"] = 0;
        $response["message"] = "Oops! An error occurred.";
 
        // echoing JSON response
        echo json_encode($response);
    }
} else {
    // required field is missing
    $response["success"] = 0;
    $response["message"] = "Required field(s) is missing";
 
    // echoing JSON response
    echo json_encode($response);
}
?>