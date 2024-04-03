<?php 
	session_start();
	require 'ClassIndividu.php';
	require 'ClassIndividuManager.php';
	
	$conn = new PDO("mysql:host=localhost;dbname=projetweb", 'root', '' , array(PDO::MYSQL_ATTR_INIT_COMMAND => 'SET NAMES utf8'));
	$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	$maBDDIndividu = new IndividuManager($conn);
	
	//verification
	if(isset($_GET['nom']) && $maBDDIndividu->verifierIndividu($_GET['nom']))
	{
		$nomIndividu = $_GET['nom'] ;
		$IndividuAAfficher = $maBDDIndividu->get($nomIndividu);	
	}
	
	else
	{
		$IndividuAAfficher = $maBDDIndividu->randomIndividu() ;
		$nomIndividu = $IndividuAAfficher->getNom() ;
	}
	
	if($IndividuAAfficher->getRole() == 'Acteur')
			$Filmographie = $maBDDIndividu->getListeFilmActeur($nomIndividu);
		
		else
			$Filmographie = $maBDDIndividu->getListeFilmRealisateur($nomIndividu);
	
?>
<!DOCTYPE html>
<html lang="fr">
<head>

<title><?php echo $IndividuAAfficher->getNom(); ?></title>  

<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

<style> #photoindividu {
    display:block;
width:400px;
height:400px;
background-position:50% 50%;
object-fit:contain ;
}

img {
display:block;
width:200px;
height:400px;
background-position:50% 50%;
}
 
</style>
</head>	
<body>
<?php include 'navigation.php'; ?>


<!-- gros div -->
<div class="row m-5">
    
	<!-- div image -->
	<div class="col-sm-6">
	   
	<div class="text-center">
	<br>
	<img src="<?php echo $IndividuAAfficher->getPhoto(); ?>" class="img-fluid rounded  mx-auto d-block" alt="<?php echo $IndividuAAfficher->getNom(); ?>" id="photoindividu">
	</div>
	</div>
	<!-- div image -->
	
	<!-- div info -->
	<div class="col-sm-6">
	<br>
	<h3><?php echo $IndividuAAfficher->getNom(); ?></h3>
<p>
Date de naissance : <?php setlocale(LC_TIME, "french");
echo utf8_encode(strftime("%d %B %G", strtotime($IndividuAAfficher->getDateDeNaissance()))); ?> <br><br> 


Nationalité : <?php echo locale_get_display_region('-' .$IndividuAAfficher->getNationalite(), 'fr'); ?> <br><br>
Role :  <?php echo $IndividuAAfficher->getRole(); ?> <br><br>
Biographie : <?php echo $IndividuAAfficher->getBiographie(); ?>
</p>
</div>
</div>

<br> 
<div class="m-5">
<h4>Filmographie</h4>
<!-- https://bbbootstrap.com/snippets/bootstrap-4-owl-carousel-for-user-testimonials-15176428 -->
<div class="row">
    <?php 
	
	foreach($Filmographie as $film )
	{
		echo ' <div class="col-lg-3 col-md-6 col-sm-6"> 
      <div class="mb-3">
        <a href="film.php?nom=' . rawurlencode($film->getTitre()). '">
          <img src=" ' . $film->getAffiche() . ' " alt=" ' . $film->getTitre(). ' " style="width:100%">
          <div class="caption">
            <p class="text-center">' . $film->getTitre(). '</p>
          </div>
        </a>
      </div>
    </div> ' ;
		
	}
	
	?>
	
</div>
</div>

 


	
<div class="m-5">
<h4>Decouvrez aussi</h4>
<?php $decouvrir = $maBDDIndividu->randomIndividus(4) ; ?>

<div class="row">

<?php 
	
foreach($decouvrir as $i)
{
	echo '<div class="col-lg-3 col-md-6 col-sm-6 mb-3">
      <div class="thumbnail">
        <a href="individu.php?nom=' . rawurlencode($i->getNom()). '">
          <img src="' . str_replace('%2F', '/', rawurlencode($i->getPhoto())) . '" alt="' . $i->getNom(). '" style="width:100%">
          <div class="">
            <p class="text-center"> ' . $i->getNom(). ' </p>
          </div>
        </a>
      </div>
    </div>' ;
}
?>
	
</div>
</div>

<?php include 'footer.html'; ?>


</body>
</html> 