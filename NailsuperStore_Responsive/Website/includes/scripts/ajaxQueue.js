var g_q; function enqueue(sUrl, postFuncCall, preFuncCall) {
    if (!g_q) g_q = new ajaxQueue; if (!g_q.enqueue(sUrl, postFuncCall, preFuncCall))
        return false; if (!g_q.isProcessing)
        g_q.process(); return true;
}
function ajaxQueue() {
    this.queue = new Array(); this.position = null; this.isProcessing; this.callPreFunctions = true; this.enqueue = function(sUrl, postFuncCall, preFuncCall) {
        for (var i = 0; i < this.queue.length; i++)
            if (this.queue[i][0] == sUrl && this.queue[i][1] == postFuncCall && this.queue[i][2] == preFuncCall) return false; if (this.position == null) { this.isProcessing = false; this.callPreFunctions = true; this.position = 0; } else { ++this.position; }
        this.queue[this.position] = new Array(); this.queue[this.position][0] = sUrl; this.queue[this.position][1] = postFuncCall; this.queue[this.position][2] = preFuncCall; if (this.isProcessing && preFuncCall)
            eval(preFuncCall); return true;
    }
    this.process = function() {
        if (this.position == null || this.queue.length == 0) return; this.isProcessing = true; if (this.callPreFunctions) {
            var r = getXMLHTTP(); var args; if (r) {
                for (var i = 0; i < this.queue.length; i++) {
                    if (this.queue[i][2])
                        eval(this.queue[i][2]);
                } 
            }
            this.callPreFunctions = false;
        }
        this.getAjaxQueueResult(this.queue[0][0], this.queue[0][1]);
    }
    this.getAjaxQueueResult = function(sUrl, postFuncCall) {
        var r = getXMLHTTP(); if (r) {
            r.open("GET", sUrl, true); r.onreadystatechange = function() { if (r.readyState == 4) { if (postFuncCall) postFuncCall(r.responseText); g_q.queue.splice(0, 1); g_q.position > 0 ? --g_q.position : g_q.position = null; if (g_q.position == null) { g_q.isProcessing = false; g_q.callPreFunctions = true; return; } else { g_q.process(); } } }
            r.send(null);
        } 
    } 
}