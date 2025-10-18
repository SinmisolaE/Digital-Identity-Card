import {Link, useLocation} from 'react-router-dom';
import {Wallet, QrCode, Shield} from "lucide-react";


const Layout = ({children}) => {
    const location = useLocation();

    const isActive = (path) => location.pathname === path;

    return (
        <div>
            <header>
                <div>
                    <div>
                        <div>
                            <div>
                                <span>Citizens Wallet</span>
                            </div>
                        </div>
                    </div>
                </div>
            </header>


            <main>
                {children}
            </main>

            <nav>
                <div>
                    <div>
                        <Link>
                            <span>Receive</span>
                        </Link>

                        <Link>
                            <span>Verify</span>
                        </Link>
                    </div>
                </div>
            </nav>
        </div>
    )

}

export default Layout;