export default function headersOutput(r) {
    // r.log('Hello from Javascript');

    // r.status = 200;
    r.headersOut.foo = 1234;
    r.headersOut['Content-Type'] = "text/plain; charset=utf-8";
    // r.headersOut['Content-Length'] = 15;
    // r.sendHeader();
    // r.send("nginx");
    // r.send("java");
    // r.send("script");
    // r.finish();

    r.return(200, JSON.stringify(r));
}