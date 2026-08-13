-- =============================================
-- Ejercicio 2: Consultas SQL y Optimización
-- =============================================

-- 1. Consulta del usuario que más tiempo ha estado logueado
WITH RankedLogins AS (
    SELECT User_id, TipoMov, fecha,
           ROW_NUMBER() OVER (PARTITION BY User_id ORDER BY fecha) as rn
    FROM ccloglogin
),
MatchedPairs AS (
    SELECT l1.User_id, l1.fecha as LoginTime, l2.fecha as LogoutTime,
           CAST(DATEDIFF(SECOND, l1.fecha, l2.fecha) AS BIGINT) as SessionDurationSeconds
    FROM RankedLogins l1
    JOIN RankedLogins l2 ON l1.User_id = l2.User_id AND l1.rn = l2.rn - 1
    WHERE l1.TipoMov = 1 AND l2.TipoMov = 0
),
UserTotals AS (
    SELECT User_id, SUM(SessionDurationSeconds) as TotalSeconds
    FROM MatchedPairs
    GROUP BY User_id
)
SELECT TOP 1 
    User_id, 
    CONCAT(
        TotalSeconds / 86400, ' días, ', 
        (TotalSeconds % 86400) / 3600, ' horas, ', 
        (TotalSeconds % 3600) / 60, ' minutos, ', 
        TotalSeconds % 60, ' segundos'
    ) AS [Tiempo total]
FROM UserTotals
ORDER BY TotalSeconds DESC;

-- =============================================
-- 2. Consulta del usuario que menos tiempo ha estado logueado
WITH RankedLogins AS (
    SELECT User_id, TipoMov, fecha,
           ROW_NUMBER() OVER (PARTITION BY User_id ORDER BY fecha) as rn
    FROM ccloglogin
),
MatchedPairs AS (
    SELECT l1.User_id, l1.fecha as LoginTime, l2.fecha as LogoutTime,
           CAST(DATEDIFF(SECOND, l1.fecha, l2.fecha) AS BIGINT) as SessionDurationSeconds
    FROM RankedLogins l1
    JOIN RankedLogins l2 ON l1.User_id = l2.User_id AND l1.rn = l2.rn - 1
    WHERE l1.TipoMov = 1 AND l2.TipoMov = 0
),
UserTotals AS (
    SELECT User_id, SUM(SessionDurationSeconds) as TotalSeconds
    FROM MatchedPairs
    GROUP BY User_id
)
SELECT TOP 1 
    User_id, 
    CONCAT(
        TotalSeconds / 86400, ' días, ', 
        (TotalSeconds % 86400) / 3600, ' horas, ', 
        (TotalSeconds % 3600) / 60, ' minutos, ', 
        TotalSeconds % 60, ' segundos'
    ) AS [Tiempo total]
FROM UserTotals
ORDER BY TotalSeconds ASC;

-- =============================================
-- 3. Promedio de logueo por mes
WITH RankedLogins AS (
    SELECT User_id, TipoMov, fecha,
           ROW_NUMBER() OVER (PARTITION BY User_id ORDER BY fecha) as rn
    FROM ccloglogin
),
MatchedPairs AS (
    SELECT l1.User_id, l1.fecha as LoginTime, l2.fecha as LogoutTime,
           CAST(DATEDIFF(SECOND, l1.fecha, l2.fecha) AS BIGINT) as SessionDurationSeconds
    FROM RankedLogins l1
    JOIN RankedLogins l2 ON l1.User_id = l2.User_id AND l1.rn = l2.rn - 1
    WHERE l1.TipoMov = 1 AND l2.TipoMov = 0
),
UserMonthlyAverages AS (
    SELECT User_id, 
           YEAR(LoginTime) AS Anio, 
           MONTH(LoginTime) AS Mes,
           AVG(SessionDurationSeconds) as AvgSeconds
    FROM MatchedPairs
    GROUP BY User_id, YEAR(LoginTime), MONTH(LoginTime)
)
SELECT 
    CONCAT(
        'Usuario ', a.User_id, ' en ',
        DATENAME(month, DATEADD(month, a.Mes - 1, CAST('2000-01-01' AS datetime))), ' ', a.Anio, ': ',
        a.AvgSeconds / 86400, ' días, ', 
        (a.AvgSeconds % 86400) / 3600, ' horas, ', 
        (a.AvgSeconds % 3600) / 60, ' minutos, ', 
        a.AvgSeconds % 60, ' segundos'
    ) AS PromedioLogueo
FROM UserMonthlyAverages a
-- JOIN ccUsers u ON a.User_id = u.User_id -- Descomentar si se quiere usar el "Login" en vez de User_id en el texto
ORDER BY a.Anio, a.Mes, a.User_id;
