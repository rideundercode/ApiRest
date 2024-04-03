<?php 
	session_start();
	require 'ClassUtilisateur.php';
	require 'ClassUtilisateurManager.php';
	
	if(@!$_SESSION['isConnected'])
		header('Location:index.php');
	
	$conn = new PDO("mysql:host=localhost;dbname=projetweb", 'root', '' , array(PDO::MYSQL_ATTR_INIT_COMMAND => 'SET NAMES utf8'));
	$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
		
	$maBDDUser = new UtilisateurManager($conn);
	$util = $maBDDUser->get($_SESSION['pseudo']);
?>
<!DOCTYPE html>
<html lang="fr">
<head>

<title><?php echo $util->getPseudo(); ?></title>
<link rel="stylesheet" href="index.css">

<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
<script src="parametre.js"></script>

<style> 
#imageProfile  {
    width: 200px;
    height: 200px;
}

</style>
</head>	

<body>
<?php include 'navigation.php'; ?>

<br>
<!-- gros div -->
<div class="form-group row m-5 parametre">
    
	<!-- div image -->
	<div class="col-sm-12 col-lg-6">
    
	<div class="text-center">

	<h2>Photo de profil</h2>
	<img src="<?php echo $util->getPFP(); ?>" class="img-thumbnail rounded-circle mx-auto d-block" id="imageProfile" alt="<?php echo $util->getPseudo(); ?>">
	 
	</div>
	
	</div>
	<!-- div image -->
	
	<!-- div form -->
	<div class="col-sm-12 col-lg-6 mt-3">
	
	<form>
  
	<div class="form-group row ">
    
	<label for="nom" class="col-sm-3 col-form-label">Nom</label>
		<div class="col-sm-7">
			<input type="text" readonly class="form-control-plaintext" id="nom" value="<?php echo $util->getNom(); ?>">
		</div>
	
		<div class="col-sm-2">
			<label for="nom"><a id="nomMod" href="#">Modifier</a></label>
		</div>
	</div>
  
  
  <div class="form-group row">
    <label for="prenom" class="col-sm-3 col-form-label">Prenom</label>
    <div class="col-sm-7">
      <input type="text" readonly class="form-control-plaintext" id="prenom" value="<?php echo $util->getPrenom(); ?>">
	</div>
	<div class="col-sm-2">
	<label for="prenom"><a href="#" id="prenomMod">Modifier</a></label>
	</div>
  </div>
  
  <div class="form-group row">
    <label for="dob" class="col-sm-3 col-form-label">Date de naissance</label>
    <div class="col-sm-7">
      <input type="date" readonly class="form-control-plaintext" id="dob" value="<?php echo $util->getDateDeNaissance(); ?>">  
	</div>
	<div class="col-sm-2">
	<label for="dob"><a href="#" id="dobMod">Modifier</a></label>
	</div>
  </div>
  
  <div class="form-group row">
    <label for="mail" class="col-sm-3 col-form-label">Email</label>
    <div class="col-sm-7">
      <input type="email" readonly class="form-control-plaintext" id="mail" value="<?php echo $util->getEmail(); ?>">  
	</div>
	<div class="col-sm-2">
	<label for="mail"><a href="#" id="emailMod">Modifier</a></label>
	</div>
  </div>
 
  
  <div class="form-group row">
    <label for="pass" class="col-sm-3 col-form-label">Mot de passe</label>
    <div class="col-sm-7">
      <input type="password" readonly class="form-control-plaintext" id="pass" value="<?php echo $util->getMotDePasse(); ?>">  
	</div>
	<div class="col-sm-2">
	<label for="pass"><a href="#" id="passMod">Modifier</a></label>
	</div>
  </div>
 
	</form>
  </div>

</div>

<br>
<br> 
<div class="text-center">
<button type="button" class="btn btn-danger trigger-btn" data-toggle="modal" data-target="#supprimer">Supprimer compte</button>
</div>

<div id="supprimer" class="modal fade">
	<div class="modal-dialog modal-confirm">
		<div class="modal-content">
			<div class="modal-header">			
				<h4 class="modal-title">Confirmation supprimer compte</h4>	
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
			</div>
			<div class="modal-body">
				<p>Vous etes sur ?</p>
			</div>
			<div class="modal-footer ">
				<button type="button" class="btn btn-secondary" data-dismiss="modal">Annuler</button>
				<button type="button" class="btn btn-danger" id="delete_compte">Supprimer</button>
   
			</div>
		</div>
	</div>
</div>

<br>
<br>
<?php include 'footer.html'; ?>
</body>
</html> 
