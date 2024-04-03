<?php 
	session_start(); 
	require 'ClassFilm.php';
	require 'ClassFilmManager.php';
	require 'ClassCommentManager.php';
	require_once 'ClassUtilisateurManager.php';
	
	$conn = new PDO("mysql:host=localhost;dbname=projetweb", 'root', '' , array(PDO::MYSQL_ATTR_INIT_COMMAND => 'SET NAMES utf8'));
	$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	
	$maBDDFilm = new FilmManager($conn);
	
	if(isset($_GET['nom']) && $maBDDFilm->verifierFilm($_GET['nom']))
	{
		$nomFilm = $_GET['nom'] ;
		$filmAAfficher = $maBDDFilm->get($nomFilm);
	}
	
	else
	{
		$filmAAfficher = $maBDDFilm->randomFilm() ;
		$nomFilm = $filmAAfficher->getTitre();
	}
	
	$listeActeurFilm = $maBDDFilm->getListFilmCOActeur($nomFilm);
	
	
	$maBDDCommentaire = new CommentManager($conn);
	$maBDDUser = new UtilisateurManager($conn);
	
	$listeCommentaire = $maBDDCommentaire->getListeCommentaire($nomFilm);
	$noteFilm = $maBDDCommentaire->getNoteFilm($nomFilm) ;
	$filmSimilaire = $maBDDFilm->randomFilmCategorie($filmAAfficher->getCategorie(), 4) ;
	
?>
<!DOCTYPE html>
<html lang="fr">
<head>

<title><?php echo $filmAAfficher->getTitre(); ?></title>

  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
  <link rel="stylesheet" href="film.css">
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
  <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
  <script src="comment.js"></script>

</head>	
<body>

<?php include 'navigation.php'; ?>

<!-- gros div -->
<div class="row m-5">
    
	<!-- div image -->
	<div class="col-md-6 col-sm-12">   
	<div class="text-center">
	<br>
	<img src="<?php echo str_replace('%2F', '/', rawurlencode($filmAAfficher->getAffiche())); ?>"  id="img_perc" class="rounded mx-auto d-block" alt="<?php echo $filmAAfficher->getTitre(); ?>" >
	<div id="note_jquery">
	<div class="rating"><?php for($j=0;$j<$noteFilm;$j++){ echo'&#9733;'; } ?> </div> <?php echo 'Note moyenne : ' . $noteFilm . '/5 '; ?> 
	</div> 	
	</div>
	</div>
	<!-- div image -->
	
	<!-- div form -->
	<div class="col-md-6 col-sm-12">
	<br>
	<h3 id="titre_film"><?php echo $filmAAfficher->getTitre(); ?></h3>
<p><?php echo 'Categorie : '. $filmAAfficher->getCategorie() . '</p><p> Annee : ' . $filmAAfficher->getAnnee() . '</p><p> Duree '. $filmAAfficher->getDuree() . ' minutes'  ; ?> </p>

<p> De : <?php echo '<a href="individu.php?nom=' . rawurlencode($filmAAfficher->getRealisateur()). '">' . $filmAAfficher->getRealisateur(). '</a>'; ?> </p> 
<p> Avec : <?php 

foreach($listeActeurFilm as $a)
{ 
	if($a == end($listeActeurFilm)) 
		echo '<a href="individu.php?nom=' . rawurlencode($a). '">' . $a . '</a>. ' ; 
	else 
		echo '<a href="individu.php?nom=' . rawurlencode($a). '">' . $a . '</a>, ' ;} 
?> </p>

<p>Synopsis : <?php echo $filmAAfficher->getSynopsis() ; ;?> </p>
</div>
</div>

<div class="m-5">
<!-- premiere ligne -->
	  
<div class="row mb-3">
    
	<div class="col-md-6">
      <h4>Espace commentaire </h4>
    </div>
    
	
	<?php
		
		if(@$_SESSION['isConnected'] && !$maBDDCommentaire->verifierNote($nomFilm,$_SESSION['pseudo']) )
		{ echo '
			<div class="col-md-6 text-right" id="bouton_ajouter">
		 
		 <a href="#" class="link-secondary" data-toggle="modal" data-target="#ajoutcommentaire">Ajouter commentaire </a>
		 
		</div> ' ;}
	?>

<!-- Modal ajouter commentaire -->

<div class="modal" id="ajoutcommentaire">
  <div class="modal-dialog modal-dialog-centered" role="document">
    <div class="modal-content" style="background-color :#343a40; color: #FFE77AFF;">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">Nouveau commentaire</h5>
      </div>
	  <form id="form_comment" action="#" method="POST">

      <div class="modal-body">
        
          <div class="form-group row">
		  
		  <div class="col-md-2">
            <label class="col-form-label">Notation</label>
			  </div>
          
			<div class="star-rating col-md-4">

            
  <input type="radio" id="5-stars" name="rating" value="5" required />
  <label for="5-stars" class="star">&#9733;</label>
  <input type="radio" id="4-stars" name="rating" value="4" required />
  <label for="4-stars" class="star">&#9733;</label>
  <input type="radio" id="3-stars" name="rating" value="3" required />
  <label for="3-stars" class="star">&#9733;</label>
  <input type="radio" id="2-stars" name="rating" value="2" required />
  <label for="2-stars" class="star">&#9733;</label>
  <input type="radio" id="1-star" name="rating" value="1" required />
  <label for="1-star" class="star">&#9733;</label>
</div>
          </div>
		  
		  
          <div class="form-group">
            <label for="commentaire" class="col-form-label">Commentaire</label>
            <textarea class="form-control" name="commentaire" id="commentaire" required style="background-color :#FFE77AFF;"></textarea>
          </div>
        
      </div>
      <div class="modal-footer">
        <button type="submit" class="btn btn-secondary" style="color:#FFE77AFF; ">Ajouter commentaire</button>
		<button type="button" class="btn btn-secondary" data-dismiss="modal" style="color:#FFE77AFF; ">Annuler</button>
      </div>
	  </form>
    </div>
  </div>
</div>


	
</div>
 
<!-- fin premiere ligne -->

<!-- deuxieme ligne afficher commentaire -->
<div id="place_commentaire"></div>

<?php 

foreach( $listeCommentaire as $i)
{
echo '

 
 <div class="d-flex justify-content-center row mb-4">
 <div class="col-md-12"  >
 <div class="card-header pt-1 pb-1 " style="background-color :#343a40; " >
	<div class="media flex-wrap w-100 align-items-center " > 
	<img src="' . $maBDDUser->get($i->getPseudo())->getPFP() . '" class="rounded-circle" alt="" width="64" height="64">
                         <div class="media-body ml-3" > <p style="color : #FFE77AFF;">' . $i->getPseudo() . '</p>
                             <div class="rating">'; for($j=0;$j<$i->getNoteUtilisateur();$j++){ echo'&#9733;';} echo '</div>
                         </div>
                         <div class="text-muted small ml-3">
                             <div style="color : #FFE77AFF;">'. $i->getDateNote() . '</div>   
                         </div>
                     </div>
                 </div>
				 
                    <div class= "p-1" style="  background-color :#343a40 ; color : #FFE77AFF; " >
                       	'. $i->getComment() . '	
                     </div>
</div>
</div>
' ; }
  ?>
</div>

<div class="m-5">
<h4 class="mb-3">Films similaires </h4>
 <div class="row ">
	<?php 
	
	foreach( $filmSimilaire as $i)
	{
		echo '
    <div class="col-md-3 col-sm-6">
      <div class="mb-3">
        <a href="?nom='. rawurlencode($i->getTitre()). ' ">
          <img class="film_similaire" src="'. str_replace('%2F', '/', rawurlencode($i->getAffiche())). '" alt="'.$i->getTitre().'" style="width:100%">
          <div>
            <p class="text-center">'. $i->getTitre(). '</p>
          </div>
        </a>
      </div>
    </div>	
	' ; 
	}	
?>
</div>

</div>

<?php include 'footer.html'; ?>
</body>
</html> 