SELECT DISTINCT Addresses.Name
FROM Addresses INNER JOIN
	(Poisoners INNER JOIN
		(P_Ps INNER JOIN Poisons
		ON P_Ps.PoisonId = Poisons.Id)
	ON Poisoners.Id = P_Ps.PoisonerId
	)
	ON Addresses.Id = Poisoners.AddressId
WHERE(Poisons.Name = PoisonName);