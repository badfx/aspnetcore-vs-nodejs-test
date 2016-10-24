const restify = require('restify');  
const server = restify.createServer();
const corsMiddleware = require('restify-cors-middleware');
const cors = corsMiddleware({  
    allowHeaders: ['Authorization']
});

//setup CORS
server.pre(cors.preflight);  
server.use(cors.actual);  

server.use(restify.queryParser());  
server.use(restify.bodyParser());  

//custom middleware to log time performance of requests
function myTimeLoggerMiddleware(req, res, next) {  
    var startTime = new Date().getTime();

    res.on('finish', () => {
        var endTime = new Date().getTime();
        console.log('Response generation length:', endTime - startTime, 'msec');
    });

    next();
}

server.use(myTimeLoggerMiddleware);

//listen on port 5000
server.listen(5000);

//server REST API methods
server.get('/api/customer/list', (req, res) => {  
    customerService.list()
        .then(
            customerList => res.json(customerList),
            err => res.send(500, err)
        );
});

server.post('/api/customer', (req, res) => {  
    customerService.create(req.body.firstName, req.body.lastName)
        .then(
            () => res.send(200),
            err => res.send(500, err)
        );
});

server.del('/api/customer/:id', (req, res) => {  
    customerService.remove(req.params.id)
        .then(
            () => res.send(200),
            err => res.send(500, err)
        );
});