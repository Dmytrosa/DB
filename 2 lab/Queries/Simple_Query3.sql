SELECT DISTINCT Poisoners.Name
FROM Poisoners
WHERE CountOfPoisons <
	(SELECT COUNT(P_Ps.PoisonId)
	 FROM P_Ps 
	 WHERE P_Ps.PoisonerId = Poisoners.Id);