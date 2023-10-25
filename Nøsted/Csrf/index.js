app.get('/', csrfProtection, (req, res) => {
    res.render('index', { csrfToken: req.csrfToken() });
})