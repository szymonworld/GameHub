<?php
 
require_once __DIR__ . '/db_connect.php';
 
$response = array();
$requestType = array("createGame","createAccount","getAccountByLogin");
$selectedType = null;
$conn = null;

if(checkType())
{
	parseType();
}

function checkType()
{
	global $conn;
	for ($i = 0; $i < 3; $i++) 
	{
		if($_GET['type'] == $i)
		{
			global $selectedType;
			$selectedType = $i; 
			
			try 
			{
				$db = new DB_CONNECT();
				$conn = $db->connect();
			} 
			catch(PDOException $e)
			{
				jsonResponse(0, 'Connection failed'.$e->getMessage());
				return false;
			}
			
			$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
			return true;
		} 
	}
	jsonResponse(0, "Cannot recognize request type.");
	return false;
}

function parseType()
{
	global $selectedType;
	
	switch ($selectedType) 
	{
		case 0:
			createAccount($_GET['login'], $_GET['password'], $_GET['firstname'], $_GET['lastname']);
			break;
		case 1:
			createEvent($_GET['name'], $_GET['desc'], $_GET['date'], $_GET['gameid']);
			break;
		case 2:
			getAccountByLogin($_GET['login']);
			break;
		case 3:
			createEvent($_GET['login'], $_GET['password'], $_GET['eventName'], $_GET['description'], $_GET['date'], $_GET['game']);
			break;
		case 4:
			getEvent($_GET['login'], $_GET['password'], $_GET['eventid']);
			break;
		case 5:
			addEvent($_GET['login'], $_GET['password'], $_GET['eventName'], $_GET['gameid'], $_GET['description'], $_GET['date']);
			break;
		case 6:
			getEventParticipants($_GET['login'], $_GET['password'], $_GET['eventid']);
			break;
		case 7:
			addEventParticipant($_GET['login'], $_GET['password'], $_GET['eventid'], $_GET['accountid']);
			break;
		case 8:
			getFriends($_GET['login'], $_GET['password']);
			break;
		case 9:
			addFriend($_GET['login'], $_GET['password'], $_GET['accountid']);
			break;
		case 10:
			deleteFriend($_GET['login'], $_GET['password'], $_GET['accountid']);
			break;
		case 11:
			getInvitations($_GET['login'], $_GET['password']);
			break;
		case 12:
			addInvitation($_GET['login'], $_GET['password'], $_GET['accountid'], $_GET['eventid']);
			break;
		case 13:
			deleteInvitation($_GET['login'], $_GET['password'], $_GET['inviteid']);
			break;
		default:
			break;
		
	}
}
 
 
function authenticate($login, $password)
{
	//return password_verify($password, loadHashByLogin($login));
	return $password == loadHashByLogin($login);
}
 
function loadHashByLogin($login)
{
	global $conn;
	$query = $conn->prepare("SELECT Password FROM Accounts WHERE Login = :login");
	$query->bindValue(':login', $login);
	$query->execute();
	return $query->fetch();
}

function isLoginExist($login)
{
	global $conn;
	$query = $conn->prepare("SELECT count(*) FROM Accounts WHERE Login = :login"); 
	$query->bindValue(":login",$login);
	$query->execute(); 
	return $query->fetchColumn() > 0; 	
}

function jsonResponse($success, $message)
{
	$response["success"] = $success;
	$response["message"] = $message;	
	echo json_encode($response);	
}

function createEvent($login, $password, $eventName, $description, $date, $game)	 
{
	if(authenticate($login, $password))
	{	
		try 
		{  
			global $conn;
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Events(EventName, Description, Date, GameID) VALUES(:eventName, :description, :date, :game)");
			$query->bindValue(':eventName', $eventName);
			$query->bindValue(':description', $description);
			$query->bindValue(':date', $date);
			$query->bindValue(':game', $game);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Event created.");	
	  
		} 
		catch (Exception $e) 
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
			
	}
	else
	{
		jsonResponse(0, "Authentification error.");	
	}
	
}

function getEvent($login, $password, $eventID)
{
	if(authenticate($login, $password))
	{	
		global $conn;
		$query = $conn->prepare("SELECT * FROM Events WHERE EventID = :eventID"); 
		$query->bindValue(":eventID",$eventID);
		$query->execute(); 		
		$fetch = $query->fetch();
		
		$response['EventName'] = $fetch['EventName'];
		$response['GameID'] = $fetch['GameID'];
		$response['Description'] = $fetch['Description'];
		$response['Date'] = $fetch['Date'];
		
		echo json_encode($response);
	}
	
}

function addEvent($login, $password, $eventName, $gameID, $description, $date)
{
	if(authenticate($login, $password))
	{	
		
		try 
		{  
			global $conn;		
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Events(EventName, GameID, Description, Date) 
									 VALUES(:eventName, :gameID, :description, :date)");
									 
			$query->bindValue(':eventName', $eventName);
			$query->bindValue(':gameID', $gameID);
			$query->bindValue(':description', $description);
			$query->bindValue(':date', $date);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Event created.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
}

function getEventParticipants($login, $password, $eventID)
{
	if(authenticate($login, $password))
	{	
		global $conn;
		$query = $conn->prepare("SELECT * FROM EventParticipants WHERE EventID = :eventID"); 
		$query->bindValue(":eventID",$eventID);
		$query->execute(); 		
		
		while($fetch = $query->fetch())
		{
			$response['ParticipationID'] = $fetch['ParticipationID'];
			$response['AccountID'] = $fetch['AccountID'];
			echo json_encode($response);
		}	

	}	
}

function addEventParticipant($login, $password, $eventID, $accountID)
{
	if(authenticate($login, $password))
	{	
		
		try 
		{  
			global $conn;		
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO EventParticipants(EventID, AccountID) 
									 VALUES(:eventID, :accountID)");
									 
			$query->bindValue(':eventID', $eventID);
			$query->bindValue(':accountID', $accountID);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Event participant added.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
}

function getFriends($login, $password)
{
	if(authenticate($login, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Login = :login");
		$query->bindValue(':login', $login);
		$query->execute();
		$accountID = $query->fetch();
		
		$query = $conn->prepare("SELECT * FROM Friends WHERE AccountID1 = :accountID"); 
		$query->bindValue(":accountID",$accountID['AccountID']);
		$query->execute(); 		
		
		while($fetch = $query->fetch())
		{
			$response['FriendshipID'] = $fetch['FriendshipID'];
			$response['AccountID2'] = $fetch['AccountID2'];
			echo json_encode($response);
		}	
		
	}	
}

function addFriend($login, $password, $AccountID2)
{
	if(authenticate($login, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Login = :login");
		$query->bindValue(':login', $login);
		$query->execute();
		$accountID = $query->fetch();
		
		try 
		{  	
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Friends(AccountID1, AccountID2) 
									 VALUES(:accountID1, :accountID2)");
									 
			$query->bindValue(':accountID1', $accountID['AccountID']);
			$query->bindValue(':accountID2', $accountID2);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Friend added.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
}

function deleteFriend($login, $password, $AccountID2)
{
	if(authenticate($login, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Login = :login");
		$query->bindValue(':login', $login);
		$query->execute();
		$accountID = $query->fetch();
		
		try 
		{  	
			$conn->beginTransaction();
			
			$query = $conn->prepare("DELETE FROM Friends WHERE AccountID1 = :accountID1 and AccountID2 = :accountID2");
			$query->bindValue(':accountID1', $accountID['AccountID']);
			$query->bindValue(':accountID2', $accountID2);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Friend deleted.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
}

function getInvitations($login, $password)
{
	if(authenticate($login, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Login = :login");
		$query->bindValue(':login', $login);
		$query->execute();
		$accountID = $query->fetch();
		
		$query = $conn->prepare("SELECT * FROM Invitations WHERE AccountID = :accountID"); 
		$query->bindValue(":accountID",$accountID['AccountID']);
		$query->execute(); 		

		while($fetch = $query->fetch())
		{
			$response['InviteID'] = $fetch['InviteID'];
			$response['EventID'] = $fetch['EventID'];
			echo json_encode($response);
		}			
	}	
}

function addInvitation($login, $password, $AccountID, $EventID)
{
	if(authenticate($login, $password))
	{	
		global $conn;
		
		$query = $conn->prepare("SELECT AccountID FROM Accounts WHERE Login = :login");
		$query->bindValue(':login', $login);
		$query->execute();
		$accountID = $query->fetch();
		
		try 
		{  	
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Invitations(AccountID, EventID) 
									 VALUES(:accountID, :eventID)");
									 
			$query->bindValue(':accountID', $accountID['AccountID']);
			$query->bindValue(':eventID', $eventID);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Invitation added.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
}

function deleteInvitation($login, $password, $inviteID)
{
	if(authenticate($login, $password))
	{	
		global $conn;
		
		try 
		{  	
			$conn->beginTransaction();
			
			$query = $conn->prepare("DELETE FROM Invitations WHERE InviteID = :inviteID");
			$query->bindValue(':inviteID', $inviteID);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Invitation deleted.");
			
		}
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
	}
}

function createAccount($login, $password, $firstname, $lastname, $email, $mic, $pic, $status, $lang, $rep,
					   $PSN_Account, $XBOX_Account, $STEAM_Account, $ORIGIN_Account, $DISCORD_Account, $UPLAY_Account, $BATTLE_Account, $LOL_Account, $SKYPE_Account)
{
	
	if(!isLoginExist($login))
	{	
		try 
		{  
			global $conn;
			
			$conn->beginTransaction();
			
			$query = $conn->prepare("INSERT INTO Accounts(Login, Password, FirstName, LastName, Email, Microphone, ProfilePicture, Status, Language, RepPoint ) 
									 VALUES(:login, :password, :firstname, :lastname, :email, :mic, :pic, :status, :lang, :rep)");
									 
			$query->bindValue(':login', $login);
			$query->bindValue(':password', $password);
			$query->bindValue(':firstname', $firstname);
			$query->bindValue(':lastname', $lastname);
			$query->bindValue(':email', $email);
			$query->bindValue(':mic', $mic);
			$query->bindValue(':pic', $pic);
			$query->bindValue(':status', $status);
			$query->bindValue(':lang', $lang);
			$query->bindValue(':rep', $rep);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Account created.");
	  
		} 
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}
		
		try 
		{  
			global $conn;
			
			$conn->beginTransaction();
			getAccountByLogin($login);
			$query = $conn->prepare("INSERT INTO LinkAccount(PSN_Account, XBOX_Account, STEAM_Account, ORIGIN_Account, DISCORD_Account, UPLAY_Account, BATTLE_Account, LOL_Account, SKYPE_Account, AccountID ) 
									 VALUES(:PSN_Account, :XBOX_Account, :STEAM_Account, :ORIGIN_Account, :DISCORD_Account, :UPLAY_Account, :BATTLE_Account, :LOL_Account, :SKYPE_Account, :AccountID)");
									 
			$query->bindValue(':PSN_Account', $PSN_Account);
			$query->bindValue(':XBOX_Account', $XBOX_Account);
			$query->bindValue(':STEAM_Account', $STEAM_Account);
			$query->bindValue(':ORIGIN_Account', $ORIGIN_Account);
			$query->bindValue(':DISCORD_Account', $DISCORD_Account);
			$query->bindValue(':UPLAY_Account', $UPLAY_Account);
			$query->bindValue(':BATTLE_Account', $BATTLE_Account);
			$query->bindValue(':LOL_Account', $LOL_Account);
			$query->bindValue(':SKYPE_Account', $SKYPE_Account);
			$acc = getAccountByLogin($login);
			$query->bindValue(':AccountID', $acc['AccountID']);
			$query->execute();
			
			$conn->commit();
			
			jsonResponse(1, "Profile created.");
	  
		} 
		catch (Exception $e) 																																																																								
		{
			$conn->rollBack();
			jsonResponse(0, "Error.");
		}		
			
			
	}
	else
	{
		jsonResponse(0, "Login exist.");	
	}
	
}


function getAccountByLogin($login)
{
	if(isLoginExist($login))
	{
			global $conn;
			
			$query = $conn->prepare("SELECT FirstName, LastName, Email, Microphone, ProfilePicture, Status, Language, RepPoint FROM Accounts WHERE Login = :login");
			$query->bindValue(':login', $login);
			$query->execute();
			$fetch = $query->fetch();
			$response['AccountID'] = $fetch['AccountID'];
			$response['Login'] = $fetch['Login'];
			$response['FirstName'] = $fetch['FirstName'];
			$response['LastName'] = $fetch['LastName'];
			$response['Email'] = $fetch['Email'];
			$response['Description'] = $fetch['Description'];
			$response['Microphone'] = $fetch['Microphone'];		
			$response['ProfilePicture'] = $fetch['ProfilePicture'];		
			$response['Status'] = $fetch['Status'];		
			$response['Language'] = $fetch['Language'];		
			$response['RepPoint'] = $fetch['RepPoint'];		
			
			echo json_encode($response);
	}
	
}

?>